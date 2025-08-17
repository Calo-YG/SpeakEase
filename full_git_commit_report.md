# 完整Git提交历史报告

## 所有提交记录

| 提交哈希 | 作者 | 日期 | 提交信息 |
|---------|------|------|---------|
| 80939f7 | wyg | 2025-08-12 | refactor: 删除 SpeakEase.EventBus 项目中的本地和 RabbitMQ事件总线功能 |
| 94524a0 | wyg | 2025-08-05 | feat(eventbus): 添加 RabbitMQ事件总线功能 |
| 4cec131 | wyg | 2025-08-04 | feat(auth): 实现 token 刷新功能 |
| 5d1a4eb | wyg | 2025-07-31 | feat(auth): 实现基于 Token 的身份验证和授权 |
| ee9dc80 | wyg | 2025-07-30 | refactor(gateway): 重构用户相关接口和登录逻辑 |
| b1ad1e4 | wyg | 2025-07-29 | feat(user): 添加用户分页查询功能 |
| 7b66db3 | wyg | 2025-07-28 | feat(gateway): 实现系统用户基本功能 |
| 5841efd | wyg | 2025-07-24 | feat(gateway): 实现集群管理功能 |
| 2ff9afb | wyg | 2025-07-22 | feat(infrastructure): 实现自定义工作 Id 生成和 Redis扩展 |
| a0acb32 | wyg | 2025-07-18 | feat(route): 添加路由管理功能- 新增路由管理相关的实体、DTO和接口 - 实现路由创建、更新、删除和查询等功能 - 添加路由管理的API接口映射 - 优化应用管理功能，增加应用详情和下拉列表接口 |
| baa5fcb | wyg | 2025-07-17 | chore: 修改 IDE 检查配置，禁用 SqlNoDataSourceInspection 警告 |
| 29feb71 | wyg | 2025-07-17 | feat(gateway): 实现应用管理功能并重构代码 |
| f90ccc0 | wyg | 2025-06-26 | refactor(study): 重构路由映射并更新命名规范 |
| 9b8379a | wyg | 2025-06-26 | refactor(authorization): 重构权限管理模块 |
| a352805 | wyg | 2025-06-26 | refactor(study): 重构应用层命名空间并修正部分代码- 将 SpeakEase.Application.Auth重命名为 SpeakEase.Study.Application.Auth - 将 SpeakEase.Application.User 重命名为 SpeakEase.Study.Application.User - 修正部分变量命名和拼写错误 - 更新项目引用和命名空间 |
| e813e1e | wyg | 2025-06-26 | refactor(framework): 优化依赖注入扩展方法 |
| 9a0f348 | wyg | 2025-06-26 | feat(gateway): 添加网关实体并实现分页功能 |
| c4b7c70 | wyg | 2025-06-26 | build(SpeakEase.Gateway): 添加应用层项目引用 |
| af2f5f3 | wyg | 2025-06-26 | feat(gateway): 添加 Scalar API 文档和健康检查接口 |
| 79e571a | wyg | 2025-06-25 | feat(gateway): 初始化网关服务配置和数据库结构 |
| 8b70e79 | wyg | 2025-06-25 | refactor: 修改数据库连接字符串为占位符 |
| 438d576 | wyg | 2025-06-25 | feat(gateway): 添加集群和路由实体 |
| 41a54d2 | wyg | 2025-06-25 | feat(docker-yml): 添加 Nacos 配置文件 |
| c2873e0 | wyg | 2025-06-24 | feat(gateway): 添加系统用户实体及路由实体构造函数 |
| dad1f79 | wyg | 2025-06-24 | refactor(gateway): 修正 SpeakEase.Gateway.Application 项目的引用路径 |
| d34de58 | wyg | 2025-06-24 | refactor(SpeakEase.Gateway): 重构项目引用和解决方案结构- 在 SpeakEase.Gateway.csproj 中添加 SpeakEase.Gateway.Contract 的引用 - 修正 SpeakEase.Gateway.Domain.csproj 中 SpeakEase.Domain.Contract 的引用路径 - 更新 SpeakEase.Gateway.Infrastructure.csproj 中的引用 - 调整 SpeakEase.sln 中的项目顺序和 GUID |
| 2b9953c | wyg | 2025-06-24 | refactor(gateway): 重构网关服务代码结构 |
| f027299 | wyg | 2025-06-24 | feat(gateway): 添加 Entity Framework Core 支持 |
| ce7f7e9 | wyg | 2025-06-24 | feat(Redis): 丰富 Redis服务接口和实现 |
| 598463c | wyg | 2025-06-24 | feat(gateway): 新增网关相关实体和服务 |
| 18d8d93 | wyg | 2025-06-11 | 修正报错，修复类型匹配问题 |
| c3b8133 | wenyaoguang | 2025-05-27 | 添加Domain.Contract |
| 3e358f2 | wenyaoguang | 2025-05-26 | 添加相关实体 |
| e65b822 | wenyaoguang | 2025-05-26 | 统一 ID 类型为字符串并新增多个实体类 |
| a283f33 | wenyaoguang | 2025-05-26 | 删除多个实体类及其数据库表逻辑 |
| a4ceffd | wenyaoguang | 2025-05-26 | 更新数据库连接字符串 |
| 8f496b2 | wenyaoguang | 2025-05-26 | Revert "更新数据库连接设置及其他文件变更" |
| 9573313 | wenyaoguang | 2025-05-26 | 更新数据库连接设置及其他文件变更 |
| 0bebc57 | wenyaoguang | 2025-05-23 | 删除 LevelEnum 枚举和 WordEntity 类 |
| 5af5e56 | wenyaoguang | 2025-05-23 | 修复 DefaultGrpcServiceBuildFactory.cs 中的引用 |
| 29e10e7 | wenyaoguang | 2025-05-23 | 添加 gRPC 构建模块及相关接口和配置 |
| da53bdd | wenyaoguang | 2025-05-23 | 删除 SpeakEase.FastEndpoint 项目及相关代码 |
| c007058 | wenyaoguang | 2025-05-23 | 更新项目引用，添加 Study Host 支持 |
| c2dc454 | wenyaoguang | 2025-05-23 | Merge branch 'master' of https://github.com/Calo-YG/SpeakEase |
| 30cfd47 | wenyaoguang | 2025-05-23 | 添加快速端点项目及相关功能 |
| 6835c09 | wyg | 2025-05-09 | Merge branch 'master' of https://github.com/Calo-YG/SpeakEase |
| 824e989 | wyg | 2025-05-09 | 新增 WordEntity 类及 LevelEnum 枚举以支持单词管理 |
| 141b813 | wenyaoguang | 2025-05-09 | 添加对 SpeakEase.Socail 和 SpeakEase.Gateway 的支持 |
| 5e53ee2 | wenyaoguang | 2025-05-09 | 增强用户认证和服务功能 |
| 3b4dd69 | wyg | 2025-05-01 | 重构用户服务并移除授权服务实现 |
| f959cb4 | wyg | 2025-05-01 | 重构项目结构并优化用户与认证功能 |
| 674fb81 | wyg | 2025-04-30 | Merge branch 'master' of https://github.com/Calo-YG/SpeakEase |
| 1622fdc | wyg | 2025-04-30 | 修复 Entity.cs 文件中的格式问题 |
| 33241fa | wenyaoguang | 2025-04-29 | 优化异常处理中间件和日志配置 |
| ec4b448 | wenyaoguang | 2025-04-28 | 重构 API 文档生成与配置 |
| ccd515a | wenyaoguang | 2025-04-28 | 增强授权处理和更新解决方案配置 |
| f5dcf15 | wenyaoguang | 2025-04-28 | 重命名 ResponseBase 类中的 IsSuccess 属性 |
| db30f3a | wenyaoguang | 2025-04-25 | 更新数据库连接字符串 |
| 7165b4d | wenyaoguang | 2025-04-23 | 更新开发环境连接字符串 |
| 7286c00 | wenyaoguang | 2025-04-23 | 添加 HTTP 日志记录配置 |
| 2b9e590 | wenyaoguang | 2025-04-23 | 消除警告 |
| 6f700ae | wenyaoguang | 2025-04-23 | 添加 JWT 身份验证支持和相关功能 |
| 152fe5c | wyg | 2025-04-22 | 修改 RefreshToken 方法以返回刷新 token 响应对象 |
| 621aee1 | wyg | 2025-04-22 | 调整命名空间引用以优化依赖管理 |
| bb46d36 | wyg | 2025-04-22 | 移除对 AntiforgeryValidationException 的单独处理 |
| 439ed8e | wyg | 2025-04-22 | 添加刷新 Token 功能及相关逻辑优化 |
| 15160ca | wenyaoguang | 2025-04-21 | 添加 EntityFrameworkCore\Migrations 文件夹 |
| 916a207 | wenyaoguang | 2025-04-21 | 修复空引用和日志记录中的潜在问题 |
| 68bd98d | wyg | 2025-04-16 | 去除 UseAntiforgery |
| c42d4ff | wyg | 2025-04-16 | 修改授权options |
| cdf62d7 | wyg | 2025-04-16 | 处理静态文件加载问题问题 |
| 22193ae | wyg | 2025-04-15 | 小修改 |
| 8d894f2 | wyg | 2025-04-14 | 重构实体配置并添加用户好友相关功能 |
| 8eaa616 | wyg | 2025-04-14 | 简化AuthorizeHandler的scope管理 |
| aad6903 | wyg | 2025-04-13 | 更新 Token 过期时间和日志配置 |
| 947acdb | wyg | 2025-04-13 | 更新 CORS 配置和 AuthService 构造函数 |
| 69b6bdf | wyg | 2025-04-11 | 修改未授权状态码 |
| cafecac | wyg | 2025-04-11 | 添加swagger 注释 |
| e99a8b6 | wyg | 2025-04-11 | 解决重新命名文件夹错误 |
| 3a87541 | wyg | 2025-04-10 | 添加退出功能 |
| 0c61ed8 | wyg | 2025-04-09 | 解决获名其咎的报错 |
| 90a7045 | wyg | 2025-04-07 | 前后端联调修改 |
| c1dfac2 | wyg | 2025-04-06 | 后续增加jwt token 是否校验过期时间策略 |
| 14ce1fe | wyg | 2025-04-06 | 代码优化 |
| b4da97f | wyg | 2025-04-06 | 解决跨域前后端联调 |
| c68a298 | wyg | 2025-04-06 | 优化 |
| f563ac5 | wyg | 2025-04-06 | 优化授权方案 |
| 98b7c50 | wyg | 2025-04-05 | 修改统一响应类 |
| 655aff9 | wyg | 2025-04-05 | 修改命名 |
| a9ebf50 | wyg | 2025-04-05 | 优化授权实现 |
| a4456e4 | wyg | 2025-04-04 | 去除fastservice 引用，改用minimal api 自动注册 |
| cd7fa17 | wyg | 2025-04-01 | 添加相关实体 |
| b15e3d8 | wyg | 2025-04-01 | 添加相关实体 |
| 82fe36f | wyg | 2025-03-31 | 修改context 非跟踪查询 |
| 4f2a8ca | wyg | 2025-03-31 | 添加word level 实体 |
| 1f9a558 | wyg | 2025-03-31 | 取消迁移文件上传，修改属性默认值 |
| 291e434 | wyg | 2025-03-31 | 添加usersetting 表迁移 |
| d759b73 | wyg | 2025-03-30 | 获取当前用户请求设置 |
| 3669077 | wyg | 2025-03-30 | 添加用户设置 新增，修改 |
| f07f336 | wyg | 2025-03-30 | 代码优化 |
| fd0f570 | wyg | 2025-03-30 | 添加非跟踪查询方法 |
| a9db87d | wyg | 2025-03-30 | 完善用户文件上传 |
| d5a1a3a | wyg | 2025-03-30 | 解决报错，后续实现本地文件存储 |
| f084868 | wyg | 2025-03-30 | 后端校验到货单新增是否有采购订单以外的物料数据 |
| 1016545 | wyg | 2025-03-29 | 完善默认文件存储 |
| 20c6608 | wyg | 2025-03-29 | 移除 Contract Domain 引用，添加本地文件存储实现类 |
| 67e003a | wyg | 2025-03-29 | 修改用户密码，获取当前登录用户信息 |
| 020f5c4 | wyg | 2025-03-28 | 代码优化 |
| c50bbd4 | wyg | 2025-03-28 | 验证码改用redis 校验 |
| ae750f6 | wyg | 2025-03-28 | 添加redis 支持 |
| 37b8aca | wyg | 2025-03-28 | 登录注册 |
| 4ae80b7 | wyg | 2025-03-28 | 添加登录，注册实现 |
| 1a47d05 | wyg | 2025-03-28 | 添加验证码，修改目录结构，验证异常中间件，统一返回结果过滤器 |
| 31b7b77 | wyg | 2025-03-27 | 集成swagger |
| 5d49b50 | wyg | 2025-03-26 | 添加ITokenManager |
| 6cd7bc8 | wyg | 2025-03-26 | 修改 scproj 文件 |
| 02ee869 | wyg | 2025-03-26 | 解决efcore 迁移 |
| 23438db | wyg | 2025-03-26 | 解决运行报错 |
| 4c861bf | wyg | 2025-03-25 | 添加链接字符串和注释 |
| 13227f5 | wyg | 2025-03-25 | Merge branch 'master' of https://github.com/Calo-YG/SpeakEase |
| b69a772 | wyg | 2025-03-25 | 重写 dbcontext savechanges 方法 |
| 217ceca | wyg | 2025-03-25 | 优化dbcontext 目录位置，优化 httpcontext 获取用户信息 |
| 3059dc8 | wenyaoguang | 2025-03-20 | 异常中间件 |
| 5777ff5 | wyg | 2025-03-19 | 修改时间，改用postgresql |
| 1a44208 | wyg | 2025-03-18 | 添加 aspire 依赖 |
| be30e0c | wyg | 2025-03-18 | 修改 |
| 20d8e7c | wyg | 2025-03-17 | 修改缩进 |
| 0d0146c | wyg | 2025-03-17 | 添加项目文件、 |
| d7dd515 | wyg | 2025-03-17 | 添加 .gitattributes、.gitignore 和 README.md、 |

## 项目发展历程

### 项目初始化阶段 (2025-03-17 至 2025-03-25)
- 项目创建，添加基础文件 (.gitattributes, .gitignore, README.md)
- 集成 Swagger API 文档
- 添加数据库支持 (Entity Framework Core)
- 实现基础的用户登录注册功能
- 添加 Redis 支持
- 实现 JWT 身份验证
- 集成异常处理中间件

### 功能扩展阶段 (2025-03-26 至 2025-04-10)
- 添加用户设置和文件上传功能
- 实现好友管理功能
- 完善授权和认证机制
- 添加退出登录功能
- 解决跨域问题
- 优化 Token 刷新机制

### 架构优化阶段 (2025-04-11 至 2025-05-01)
- 重构用户服务和授权服务
- 优化项目结构和依赖管理
- 使用 Minimal API 替代 FastService
- 增强网关和社交功能支持

### 模块重构阶段 (2025-05-09 至 2025-06-26)
- 添加 gRPC 支持
- 重构权限管理模块
- 实现网关服务功能
- 添加集群管理功能
- 实现路由管理功能

### 稳定完善阶段 (2025-07-01 至 2025-08-12)
- 实现基于 Token 的身份验证和授权
- 添加用户分页查询功能
- 实现自定义工作 Id 生成和 Redis 扩展
- 添加 RabbitMQ 事件总线功能
- 最终删除本地和 RabbitMQ 事件总线功能
