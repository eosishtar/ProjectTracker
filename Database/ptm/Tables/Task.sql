CREATE TABLE [ptm].[Task] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ProjectId]    INT           NOT NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    [Description]  VARCHAR (255) NULL,
    [EffortId]     INT           NOT NULL,
    [StatusId]     INT           NOT NULL,
    [ResourceId]   INT           NOT NULL,
    [NoteId]       INT           NULL,
    [CreatedDate]  DATETIME2 (7) NOT NULL,
    [ModifiedDate] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Task_Effort] FOREIGN KEY ([EffortId]) REFERENCES [ptm].[Effort] ([Id]),
    CONSTRAINT [FK_Task_Note] FOREIGN KEY ([NoteId]) REFERENCES [ptm].[Note] ([Id]),
    CONSTRAINT [FK_Task_Project] FOREIGN KEY ([ProjectId]) REFERENCES [ptm].[Project] ([Id]),
    CONSTRAINT [FK_Task_Resource] FOREIGN KEY ([ResourceId]) REFERENCES [ptm].[Resource] ([Id]),
    CONSTRAINT [FK_Task_Status] FOREIGN KEY ([StatusId]) REFERENCES [ptm].[Status] ([Id])
);



