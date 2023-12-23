create database Test_PV
use Test_PV

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


create procedure pcd_DeleteUsers
@userID nvarchar(50)
as 
begin
delete from Users where UserID = @userID
end


-- Create a stored procedure for user search
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

CREATE PROCEDURE GetUserData
    @userid NVARCHAR(50)
AS
BEGIN
    SELECT * FROM Users WHERE UserID = @userid;
END
