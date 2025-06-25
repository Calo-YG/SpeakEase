-- sys_user 表
CREATE TABLE sys_user (
    Id VARCHAR(50) PRIMARY KEY,
    Account VARCHAR(50) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Name VARCHAR(20) NOT NULL,
    Email VARCHAR(50),
    Avatar VARCHAR(255),
    CreateById VARCHAR(50),
    CreateByName VARCHAR(50),
    CreatedAt TIMESTAMP WITH TIME ZONE,
    ModifyById VARCHAR(50),
    ModifyByName VARCHAR(50),
    ModifyAt TIMESTAMP WITH TIME ZONE
);

-- app 表
CREATE TABLE app (
    Id VARCHAR(50) PRIMARY KEY,
    AppName VARCHAR(50) NOT NULL,
    AppKey VARCHAR(50) NOT NULL,
    AppCode VARCHAR(50) NOT NULL,
    AppDescription VARCHAR(255),
    ApplyDate TIMESTAMP WITH TIME ZONE NOT NULL,
    ApplyBy VARCHAR(255),
    CreateById VARCHAR(50),
    CreateByName VARCHAR(50),
    CreatedAt TIMESTAMP WITH TIME ZONE,
    ModifyById VARCHAR(50),
    ModifyByName VARCHAR(50),
    ModifyAt TIMESTAMP WITH TIME ZONE
);

-- route 表
CREATE TABLE route (
    Id VARCHAR(50) PRIMARY KEY,
    AppId VARCHAR(50) NOT NULL,
    AppName VARCHAR(50) NOT NULL,
    Prefix VARCHAR(50) NOT NULL,
    Sort INTEGER,
    ClusterId VARCHAR(50) NOT NULL,
    AuthorizationPolicy VARCHAR(50),
    RateLimiterPolicy VARCHAR(50),
    OutputCachePolicy VARCHAR(50),
    TimeoutPolicy VARCHAR(50),
    Timeout INTERVAL,
    CorsPolicy VARCHAR(50),
    MaxRequestBodySize BIGINT,
    CreateById VARCHAR(50),
    CreateByName VARCHAR(50),
    CreatedAt TIMESTAMP WITH TIME ZONE,
    ModifyById VARCHAR(50),
    ModifyByName VARCHAR(50),
    ModifyAt TIMESTAMP WITH TIME ZONE
);

-- cluster 表
CREATE TABLE cluster (
    Id VARCHAR(50) PRIMARY KEY,
    ClusterId VARCHAR(50) NOT NULL,
    LoadBalance VARCHAR(50) NOT NULL,
    Enabled BOOLEAN NOT NULL,
    Interval INTEGER,
    Timeout INTEGER,
    Policy VARCHAR(50) NOT NULL,
    Path VARCHAR(50) NOT NULL,
    AvailableDestinationsPolicy VARCHAR(255),
    CreateById VARCHAR(50),
    CreateByName VARCHAR(50),
    CreatedAt TIMESTAMP WITH TIME ZONE,
    ModifyById VARCHAR(50),
    ModifyByName VARCHAR(50),
    ModifyAt TIMESTAMP WITH TIME ZONE
);
