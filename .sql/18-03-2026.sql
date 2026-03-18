-- Roles tabel
CREATE TABLE UserRole (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT 1 FROM UserRole WHERE Name = 'Kind')
    INSERT INTO UserRole (ID, Name) VALUES (NEWID(), 'Kind');

IF NOT EXISTS (SELECT 1 FROM UserRole WHERE Name = 'Ouder')
INSERT INTO UserRole (ID, Name) VALUES (NEWID(), 'Ouder');

-- UserData tabel
CREATE TABLE UserData (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DoctorName NVARCHAR(100),
    AppointmentDate DATETIME,
    AppointmentType NVARCHAR(100),
    UserAge INT,
    User_id NVARCHAR(450) NOT NULL,
    Role_id UNIQUEIDENTIFIER NOT NULL

    CONSTRAINT FK_UserData_User FOREIGN KEY (User_id)
        REFERENCES auth.AspNetUsers(Id),

    CONSTRAINT FK_UserData_Role FOREIGN KEY (Role_id)
        REFERENCES UserRole(Id)
);

-- Step tabel
CREATE TABLE Step (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(100),
    Description NVARCHAR(255),
    Step_order INT
);

-- Progress tabel
CREATE TABLE Progress (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserData_id UNIQUEIDENTIFIER NOT NULL,
    Step_id UNIQUEIDENTIFIER NOT NULL,
    Completed BIT DEFAULT 0,
    Completed_at DATETIME,

    CONSTRAINT FK_Progress_UserData FOREIGN KEY (UserData_id)
        REFERENCES UserData(ID),

    CONSTRAINT FK_Progress_Step FOREIGN KEY (Step_id)
        REFERENCES Step(ID)
);