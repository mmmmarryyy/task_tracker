BEGIN TRANSACTION;
ALTER TABLE [Tasks] ADD [IsArchived] bit NOT NULL DEFAULT CAST(0 AS bit);

CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [TaskId] int NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Comments_TaskId] ON [Comments] ([TaskId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250323223236_AddIsArchivedAndComment', N'9.0.3');

COMMIT;
GO

