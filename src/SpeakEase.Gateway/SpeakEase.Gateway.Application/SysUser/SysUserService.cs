using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Gateway.Contract.SysUser;
using SpeakEase.Gateway.Contract.SysUser.Dto;
using SpeakEase.Gateway.Domain.Entity.System;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Infrastructure.Exceptions;
using SpeakEase.Infrastructure.WorkIdGenerate;

namespace SpeakEase.Gateway.Application.SysUser;

/// <summary>
/// 用户服务
/// </summary>
/// <param name="dbContext">数据库上下文</param>
/// <param name="idGenerate">id生成器</param>
/// <param name="tokenManager">token管理器"></param>
public class SysUserService(IDbContext dbContext,IIdGenerate idGenerate,ITokenManager tokenManager):ISysUserService
{
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

        if (string.IsNullOrEmpty(input.Email))
        {
            throw new UserFriendlyException("请输入邮箱");
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

        if (string.IsNullOrEmpty(input.Email))
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
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<UserDetailDto> GetUserDetailAsync(string id)
    {
        return dbContext.QueryNoTracking<SysUserEntity>()
            .Where(p => p.Id == id)
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
        var user = await dbContext.QueryNoTracking<SysUserEntity>().FirstAsync(p => p.Account == input.Account);

        if (user is null)
        {
            throw new UserFriendlyException("账号密码输入错误");
        }
        
        if (user.Password != input.Password)
        {
            throw new UserFriendlyException("账号密码输入错误");
        }
        
        var clamis = new List<Claim>()
        {
            new Claim(type:UserInfomationConst.UserName,user.Name),
            new Claim(type:UserInfomationConst.UserAccount,user.Account),
            new Claim(type:UserInfomationConst.UserId,user.Id),
        };
        
        var toekn =  tokenManager.GenerateAccessToken(clamis);
        var refreshToken = tokenManager.GenerateRefreshToken();

        return new TokenDto()
        {
           Token = toekn,
            RefreshToken = refreshToken,
        };
    }
}