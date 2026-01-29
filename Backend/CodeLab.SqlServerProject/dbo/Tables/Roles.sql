CREATE TABLE [dbo].[Roles] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]          VARCHAR (10)  NOT NULL,
    [Nombre]          VARCHAR (100) NOT NULL,
    [Descripcion]     VARCHAR (255) NULL,
    [FechaCreacion]   DATETIME      DEFAULT (getdate()) NOT NULL,
    [UsuarioCreacion] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Codigo] ASC)
);
GO

