USE Shrinkr
GO

DELETE [BannedDomain]
DELETE [BannedIPAddress]
DELETE [ReservedAlias]
DELETE [BadWord]
DELETE [Visit]
DELETE [Alias]
DELETE [ShortUrl]
DELETE [User]
GO

DBCC CHECKIDENT ('User', RESEED, 0);
DBCC CHECKIDENT ('ShortUrl', RESEED, 0);
DBCC CHECKIDENT ('Alias', RESEED, 0);
DBCC CHECKIDENT ('Visit', RESEED, 0);
DBCC CHECKIDENT ('Badword', RESEED, 0);
DBCC CHECKIDENT ('ReservedAlias', RESEED, 0);
DBCC CHECKIDENT ('BannedIPAddress', RESEED, 0);
DBCC CHECKIDENT ('BannedDomain', RESEED, 0);
GO
