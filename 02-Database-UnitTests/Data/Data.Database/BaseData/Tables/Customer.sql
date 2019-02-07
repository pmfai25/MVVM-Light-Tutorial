CREATE TABLE [BaseData].[Customer] (
    [Id]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [Number] NVARCHAR (10) NOT NULL,
    [Label]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
);



