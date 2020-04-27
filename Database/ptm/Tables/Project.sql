CREATE TABLE [ptm].[Project] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    [Description]  VARCHAR (255) NULL,
    [Completed]    BIT           NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [DueDate]      DATETIME2 (7) NOT NULL,
    [CreatedDate]  DATETIME2 (7) NOT NULL,
    [ModifiedDate] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED ([Id] ASC)
);



