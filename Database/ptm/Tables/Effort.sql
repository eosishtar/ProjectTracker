CREATE TABLE [ptm].[Effort] (
    [Id]             INT          NOT NULL,
    [Description]    VARCHAR (50) NOT NULL,
    [ValueInMinutes] MONEY        NOT NULL,
    CONSTRAINT [PK_Effort] PRIMARY KEY CLUSTERED ([Id] ASC)
);

