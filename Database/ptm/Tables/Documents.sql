CREATE TABLE [ptm].[Documents] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [TaskId]   INT          NOT NULL,
    [FileName] VARCHAR (50) NOT NULL,
    [FileType] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Documents_Task] FOREIGN KEY ([TaskId]) REFERENCES [ptm].[Task] ([Id])
);

