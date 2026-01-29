CREATE TABLE [dbo].[Usuarios] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]        VARCHAR (100) NOT NULL,
    [Email]         VARCHAR (100) NOT NULL,
    [Clave]         VARCHAR (100) NOT NULL,
    [FechaCreacion] DATETIME      DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);
GO

