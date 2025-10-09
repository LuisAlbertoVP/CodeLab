CREATE PROCEDURE [dbo].[QRY_ValidarRefreshToken]
@token VARCHAR(64),
@mensaje VARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fecha DATETIME = GETDATE()

    IF NOT EXISTS (SELECT 1 FROM RefreshToken WHERE Token = @token AND FechaExpiracion > @fecha)
    BEGIN
        SET @mensaje = 'Tu sesión ha expirado, por favor vuelve a iniciar sesión.'
    END
    ELSE
    BEGIN
        DECLARE @FechaExpiracion DATETIME = DATEADD(DAY, 3, GETDATE())

        UPDATE RefreshToken
        SET FechaExpiracion = @FechaExpiracion
        WHERE Token = @token

        DECLARE @IdUsuario INT = (SELECT IdUsuario FROM RefreshToken WHERE Token = @token)

        SELECT Id, Nombre, @token AS RefreshToken, @FechaExpiracion AS FechaExpiracion
        FROM Usuarios
        WHERE Id = @IdUsuario

        SELECT r.Codigo
        FROM Roles r
        JOIN UsuarioRol ur ON ur.IdRol = r.Id
        WHERE ur.IdUsuario = @IdUsuario
    END
END
GO

