CREATE PROCEDURE [dbo].[QRY_IniciarSesion]
@email VARCHAR(100),
@clave VARCHAR(100),
@mensaje VARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Email = @email AND Clave = @clave)
    BEGIN
        SET @mensaje = 'Credenciales incorrectas'
    END
    ELSE
    BEGIN
        DECLARE @IdUsuario INT = (SELECT Id FROM Usuarios WHERE Email = @email AND Clave = @clave)
        DECLARE @RawToken VARCHAR(50) = NEWID()
        DECLARE @EncryptedToken VARBINARY(32) = HASHBYTES('SHA2_256', CONVERT(VARBINARY(50), @RawToken))
        DECLARE @TokenString VARCHAR(64) = LOWER(CONVERT(VARCHAR(64), @EncryptedToken, 2))
        DECLARE @FechaExpiracion DATETIME = DATEADD(DAY, 3, GETDATE())

        INSERT INTO RefreshToken(Id,IdUsuario,Token,FechaExpiracion)
        VALUES(@RawToken,@IdUsuario,@TokenString,@FechaExpiracion)


        SELECT Id, Nombre, @TokenString AS RefreshToken, @FechaExpiracion AS FechaExpiracion
        FROM Usuarios
        WHERE Id = @IdUsuario

        SELECT r.Codigo
        FROM Roles r
        JOIN UsuarioRol ur ON ur.IdRol = r.Id
        WHERE ur.IdUsuario = @IdUsuario
    END
END
GO

