CREATE TABLE [dbo].[RefreshToken] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [IdUsuario]         INT              NOT NULL,
    [Token]             VARCHAR (255)    NOT NULL,
    [FechaExpiracion]   DATETIME         NOT NULL,
    [FechaRevocacion]   DATETIME         NULL,
    [FechaCreacion]     DATETIME         DEFAULT (getdate()) NOT NULL,
    [FechaModificacion] DATETIME         NULL,
    [IpCreacion]        VARCHAR (50)     NULL,
    [Dispositivo]       VARCHAR (100)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios] ([Id])
);
GO

