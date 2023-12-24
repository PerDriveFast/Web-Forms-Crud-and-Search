create database Test_PV
use Test_PV

-- Create Table
create table Users
(
	UserID nvarchar(50) not null,
	UserName nvarchar(50) not null,
	Passwords nvarchar(50) null,
	Email nvarchar(50) null,
	Tel nvarchar(MAX) null,
	Disableds tinyint null
	primary key(UserID)
)
select * from Users


-- Create Procedure 

-- pcd_SaveUsers
create procedure pcd_SaveUsers
@userid nvarchar(50),
@username nvarchar(50),
@password nvarchar(50),
@email nvarchar(50),
@tel nvarchar(MAX)
as
begin 
insert into Users(UserID, UserName,Passwords, Email,Tel) values(@userid,@username, @password,@email, @tel)
end

-- pcd_UpadteUser
create procedure pcd_UpadteUser
@userid nvarchar(50),
@username nvarchar(50),
@password nvarchar(50),
@email nvarchar(50),
@tel nvarchar(MAX)
as
begin 
update Users set 
UserName = @username,
Passwords = @password,
Email= @email,
Tel= @tel
from Users where UserID = @userid
end

-- pcd_DeleteUsers
create procedure pcd_DeleteUsers
@userID nvarchar(50)
as 
begin
delete from Users where UserID = @userID
end


-- SearchUsers
CREATE PROCEDURE SearchUsers
    @SearchKeyword NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE UserName LIKE '%' + @SearchKeyword + '%'
       OR Email LIKE '%' + @SearchKeyword + '%'
       OR Tel LIKE '%' + @SearchKeyword + '%'
       OR UserID LIKE '%' + @SearchKeyword + '%'
END

-- GetUserData
CREATE PROCEDURE GetUserData
    @userid NVARCHAR(50)
AS
BEGIN
    SELECT * FROM Users WHERE UserID = @userid;
END

-- GetAllUsers
CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT * FROM Users;
END

