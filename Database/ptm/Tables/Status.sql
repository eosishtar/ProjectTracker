CREATE TABLE [ptm].[Status] (
    [Id]          INT          NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    [Inactive]    NCHAR (10)   NOT NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);

