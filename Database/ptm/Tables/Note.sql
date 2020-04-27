CREATE TABLE [ptm].[Note] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [TaskId]      INT          NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED ([Id] ASC)
);

