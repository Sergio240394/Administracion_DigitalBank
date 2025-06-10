-- Crear base de datos (opcional)
IF DB_ID('UsuariosDB') IS NULL
    CREATE DATABASE UsuariosDB;
GO

USE UsuariosDB;
GO

-- Crear tabla si no existe
IF OBJECT_ID('Usuarios', 'U') IS NULL
BEGIN
    CREATE TABLE Usuarios (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100) NOT NULL,
        FechaNacimiento DATE NOT NULL,
        Sexo CHAR(1) NOT NULL CHECK (Sexo IN ('M', 'F'))
    );
END
GO

-- Eliminar procedimiento si ya existe
IF OBJECT_ID('sp_Usuarios', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios;
GO

-- Crear procedimiento almacenado con validaciones
CREATE PROCEDURE sp_Usuarios
    @Accion NVARCHAR(20),
    @Id INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @FechaNacimiento DATE = NULL,
    @Sexo CHAR(1) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Accion = 'AGREGAR'
        BEGIN
            IF @Nombre IS NULL OR @FechaNacimiento IS NULL OR @Sexo IS NULL
            BEGIN
                RAISERROR('Debe proporcionar Nombre, FechaNacimiento y Sexo para agregar un usuario.', 16, 1);
                RETURN;
            END

            IF @Sexo NOT IN ('M', 'F')
            BEGIN
                RAISERROR('El campo Sexo debe ser "M" o "F".', 16, 1);
                RETURN;
            END

            INSERT INTO Usuarios (Nombre, FechaNacimiento, Sexo)
            VALUES (@Nombre, @FechaNacimiento, @Sexo);

            PRINT 'Usuario agregado correctamente.';
        END

        ELSE IF @Accion = 'MODIFICAR'
        BEGIN
            IF @Id IS NULL OR @Nombre IS NULL OR @FechaNacimiento IS NULL OR @Sexo IS NULL
            BEGIN
                RAISERROR('Debe proporcionar Id, Nombre, FechaNacimiento y Sexo para modificar un usuario.', 16, 1);
                RETURN;
            END

            IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Id = @Id)
            BEGIN
                RAISERROR('No existe un usuario con el Id especificado.', 16, 1);
                RETURN;
            END

            UPDATE Usuarios
            SET Nombre = @Nombre,
                FechaNacimiento = @FechaNacimiento,
                Sexo = @Sexo
            WHERE Id = @Id;

            PRINT 'Usuario modificado correctamente.';
        END

        ELSE IF @Accion = 'ELIMINAR'
        BEGIN
            IF @Id IS NULL
            BEGIN
                RAISERROR('Debe proporcionar el Id para eliminar un usuario.', 16, 1);
                RETURN;
            END

            IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Id = @Id)
            BEGIN
                RAISERROR('No existe un usuario con el Id especificado.', 16, 1);
                RETURN;
            END

            DELETE FROM Usuarios WHERE Id = @Id;

            PRINT 'Usuario eliminado correctamente.';
        END

        ELSE IF @Accion = 'CONSULTAR'
        BEGIN
            SELECT * FROM Usuarios;
        END

        ELSE IF @Accion = 'CONSULTAR_POR_ID'
        BEGIN
            IF @Id IS NULL
            BEGIN
                RAISERROR('Debe proporcionar el Id para consultar un usuario.', 16, 1);
                RETURN;
            END

            IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Id = @Id)
            BEGIN
                RAISERROR('No existe un usuario con el Id especificado.', 16, 1);
                RETURN;
            END

            SELECT * FROM Usuarios WHERE Id = @Id;
        END

        ELSE
        BEGIN
            RAISERROR('Acción no reconocida. Las acciones válidas son: AGREGAR, MODIFICAR, ELIMINAR, CONSULTAR, CONSULTAR_POR_ID.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        PRINT 'Ocurrió un error: ' + ERROR_MESSAGE();
    END CATCH
END
GO
