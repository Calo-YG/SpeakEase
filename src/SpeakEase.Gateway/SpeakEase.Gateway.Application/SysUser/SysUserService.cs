using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Gateway.Contract.SysUser;
using SpeakEase.Gateway.Contract.SysUser.Dto;
using SpeakEase.Gateway.Domain.Entity.System;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.Options;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.Infrastructure.Shared;
using SpeakEase.Infrastructure.WorkIdGenerate;

namespace SpeakEase.Gateway.Application.SysUser;

/// <summary>
/// 用户服务
/// </summary>
/// <param name="dbContext">数据库上下文</param>
/// <param name="idGenerate">id生成器</param>
/// <param name="tokenManager">token管理器"></param>
/// <param name="redisService"></param>
public class SysUserService(IDbContext dbContext,IIdGenerate idGenerate,ITokenManager tokenManager,IRedisService redisService,IOptions<JwtOptions> jwtOptions):ISysUserService
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task CreateUserAsync(CreateUserInput input)
    {
        if (string.IsNullOrEmpty(input.Name))
        {
            throw new UserFriendlyException("请输入用户名");
        }

        if (string.IsNullOrEmpty(input.Password))
        {
            throw new UserFriendlyException("请输入密码");
        }

        if (string.IsNullOrEmpty(input.Email) || !Regex.IsMatch(input.Email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
        {
            throw new UserFriendlyException("请输入邮箱正确的邮箱格式");
        }
        
        if (string.IsNullOrEmpty(input.Account))
        {
            throw new UserFriendlyException("请输入账号");
        }

        var any = await dbContext.QueryNoTracking<SysUserEntity>()
            .Where(x => x.Account == input.Account )
            .AnyAsync();

        if (any)
        {
            throw new UserFriendlyException("当前用户已存在");
        }
        var id = idGenerate.NewIdString();
        
        var user = new SysUserEntity(id, input.Account, input.Password, input.Name, input.Email, input.Avatar);

        await dbContext.SysUser.AddAsync(user);
        
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task UpdateUserAsync(UpdateUserInput input)
    {
        if (string.IsNullOrEmpty(input.Name))
        {
            throw new UserFriendlyException("请输入用户名");
        }

        if (string.IsNullOrEmpty(input.Password))
        {
            throw new UserFriendlyException("请输入密码");
        }

        if (string.IsNullOrEmpty(input.Email) || !Regex.IsMatch(input.Email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
        {
            throw new UserFriendlyException("请输入邮箱");
        }
        
        var entity = await dbContext.SysUser.FirstOrDefaultAsync(x => x.Id == input.Id);
        
        if (entity == null)
        {
            throw new UserFriendlyException("用户不存在");
        }
        
        entity.SetUserAvatar(input.Avatar);
        entity.SetUserName(input.Name);
        entity.SetUserEmail(input.Password);
        entity.SetUserEmail(input.Email);

        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task DeleteUserAsync(string id)
    {
        await dbContext.SysUser
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// 获取用户详情
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<UserDetailDto> GetUserDetailAsync()
    {
        var user = dbContext.GetUser();
        
        return dbContext.QueryNoTracking<SysUserEntity>()
            .Where(p => p.Id == user.Id)
            .Select(p => new UserDetailDto
            {
                Id = p.Id,
                Account = p.Account,
                Password = p.Password,
                Name = p.Name,
                Email = p.Email,
                Avatar = p.Email
            }).FirstAsync();
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<TokenDto> LoginAsync(LoginInput input)
    {
        var user = await dbContext.QueryNoTracking<SysUserEntity>().FirstOrDefaultAsync(p => p.Account == input.Account);  

        if(user == null)
        {
            throw new UserFriendlyException("用户不存在");
        }

        if (user is null || user.Password != input.Password)
        {
            throw new UserFriendlyException("账号密码输入错误");
        }
        
        var clamis = new List<Claim>()
        {
            new Claim(type:UserInfomationConst.UserName,user.Name),
            new Claim(type:UserInfomationConst.UserAccount,user.Account),
            new Claim(type:UserInfomationConst.UserId,user.Id),
        };
        
        var token =  tokenManager.GenerateAccessToken(clamis);
        var refreshToken = tokenManager.GenerateRefreshToken();

        var dto = new TokenDto
        {
            Token = token,
            RefreshToken = refreshToken,
        };
        
        await redisService.SetAsync(string.Format(UserInfomationConst.RedisTokenKey, user.Id), dto.Token , _jwtOptions.ExpMinutes * 60);

        await redisService.SetAsync(string.Format(UserInfomationConst.RefreshTokenKey, user.Id), dto.RefreshToken,_jwtOptions.RefreshExpire * 60);

        return dto;
    }

    /// <summary>
    /// 用户分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<PageResult<UserPageDto>> GetListAsync(UserPageInput input)
    {
        var query = dbContext.QueryNoTracking<SysUserEntity>()
            .WhereIf(!string.IsNullOrEmpty(input.UserName), x => x.Name.Contains(input.UserName))
            .WhereIf(!string.IsNullOrEmpty(input.Account),x=>x.Account.Contains(input.Account))
            .WhereIf(!string.IsNullOrEmpty(input.Email),x=>x.Email.Contains(input.Email))
            .WhereIf(input.StartTime != null, x => x.CreatedAt >= input.StartTime)
            .WhereIf(input.EndTime != null, x => x.CreatedAt <= input.EndTime)
            .OrderByDescending(p=>p.CreatedAt)
            .Select(p=> new UserPageDto
            {
                Id = p.Id,
                Account = p.Account,
                Password = p.Password,
                Name = p.Name,
                Email = p.Email,
                CreateAt = p.CreatedAt
            });

        return query.ToPageResultAsync(input.Pagination);
    }
    
    /// <summary>
    /// 推出登录
    /// </summary>
    public async Task LogOutAsync()
    {
        var user = dbContext.GetUser();
        
        var existToken = await redisService.ExistsAsync(string.Format(UserInfomationConst.RedisTokenKey, user.Id));
        
        if (existToken)
        {
            await redisService.DeleteAsync(string.Format(UserInfomationConst.RedisTokenKey, user.Id));
        }
    }
    
    /// <summary>
    /// 获取刷新token
    /// </summary>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task<string> RefreshTokenAsync(RefreshTokenInput input)
    {
        var userId = input.UserId;
        
        var refreshToken =  redisService.Get(string.Format(UserInfomationConst.RedisTokenKey, userId));
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new RefreshTokenValidateException("刷新Token已过期，请重新登录");
        }

        if (refreshToken != input.RefreshToken)
        {
            throw new RefreshTokenValidateException("刷新Token已过期，请重新登录");
        }

        var user = await dbContext.QueryNoTracking<SysUserEntity>()
            .Where(p => p.Id == userId)
            .Select(p => new 
            {
                Id = p.Id,
                Account = p.Account,
                Password = p.Password,
                Name = p.Name,
                Email = p.Email,
            })
            .FirstAsync();
        
        var token = tokenManager.GenerateAccessToken(new List<Claim>
        {
            new Claim(type:UserInfomationConst.UserName,user.Name),
            new Claim(type:UserInfomationConst.UserAccount,user.Account),
            new Claim(type:UserInfomationConst.UserId,user.Id),
        });
        
        await redisService.SetAsync(string.Format(UserInfomationConst.RedisTokenKey, user.Id), token, _jwtOptions.ExpMinutes * 60);
        
        return token;
    }
}