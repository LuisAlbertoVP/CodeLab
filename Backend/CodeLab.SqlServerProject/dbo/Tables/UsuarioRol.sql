CREATE TABLE [dbo].[UsuarioRol] (
    [IdUsuario]       INT      NOT NULL,
    [IdRol]           INT      NOT NULL,
    [FechaAsignacion] DATETIME DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC, [IdRol] ASC),
    FOREIGN KEY ([IdRol]) REFERENCES [dbo].[Roles] ([Id]),
    FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios] ([Id])
);
GO

