CREATE TABLE [dbo].[Parametros] (
    [Nombre]            VARCHAR (50)  NOT NULL,
    [Valor]             VARCHAR (MAX) NOT NULL,
    [FechaCreacion]     DATETIME      DEFAULT (getdate()) NULL,
    [FechaModificacion] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Nombre] ASC)
);
GO

