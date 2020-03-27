CREATE DATABASE Nomina
GO 

USE Nomina

CREATE TABLE TipoEmpleado (
	tipoEmpleadoID INT PRIMARY KEY IDENTITY(1,1),
	Descripcion VARCHAR(50)
)

CREATE TABLE Empleados (
	EmpleadoID INT PRIMARY KEY IDENTITY (1,1),
	Nombre VARCHAR(50) NOT NULL,
	ApellidoPaterno VARCHAR(50),
	ApellidoMaterno VARCHAR(50),
	FechaIngreso DATETIME NOT NULL,	
	Activo BIT,
	SueldoBase DECIMAL(18,2) NOT NULL,
	Prestamos DECIMAL(18,2) NOT NULL,
	DeduccionDesayuno DECIMAL(18,2) NOT NULL,
	DeduccionAhorro DECIMAL(18,2) NOT NULL,
	TarjetaGasolina DECIMAL(18,2) NOT NULL,
	tipoEmpleadoID INT NOT NULL , --(empleado/admin)
	Email VARCHAR(50) NOT NULL,
	Password VARCHAR (50) NOT NULL	
	CONSTRAINT FK_Empleados_tipoEmpleado FOREIGN KEY (tipoEmpleadoID) REFERENCES TipoEmpleado (tipoEmpleadoID)
)

CREATE TABLE ReciboNomina(
	ReciboNominaID INT PRIMARY KEY IDENTITY (1,1),
	EmpleadoID INT NOT NULL,
	Fecha DATETIME NOT NULL,
	totalDeposito DECIMAL(18,2) NOT NULL	
)

CREATE TABLE ConceptosReciboNomina(
	ConceptosReciboNominaID INT PRIMARY KEY IDENTITY (1,1),
	ReciboNominaID INT,	
	importe DECIMAL(18,2) NOT NULL,
	descripcion VARCHAR(100),
	naturaleza BIT
	CONSTRAINT FK_ConceptosReciboNomina_ReciboNomina FOREIGN KEY (ReciboNominaID) REFERENCES ReciboNomina (ReciboNominaID)
)

INSERT INTO TipoEmpleado
VALUES ('Administrador')
INSERT INTO TipoEmpleado
VALUES ('Empleado')
INSERT INTO empleados 
VALUES ('Administrador', '', '', (SELECT GETDATE()), 1, 50000.00, 0, 0, 0, 0, 1, 'admin@gmail.com', '123' )