-- test convert from char to Json
select convert(json, convert(char(300),'{}'))

-- test convert from nchar to Json
select convert(json, convert(nchar(300),'{}'))

-- test convert from varchar to Json
select convert(json, convert(varchar(max),'{}'))

-- test convert from nvarchar to Json
select convert(json, convert(nvarchar(300),'{}'))

-- test convert from Json to char
select convert(char(300), convert(json,'{}'))

-- test convert from Json to nchar
select convert(nchar(300), convert(json,'{}'))

-- test convert from Json to varchar
select convert(varchar(300), convert(json,'{}'))

-- test convert from Json to nvarchar
select convert(nvarchar(max), convert(json,'{}'))

-- test cast char to Json
select cast(cast('{}' as char(300)) as json)

-- test cast nchar to Json
select cast(cast('{}' as nchar(300)) as json)

-- test cast varchar to Json
select cast(cast('{}' as varchar(max)) as json)

-- test cast nvarchar to Json
select cast(cast('{}' as nvarchar(300)) as json)

-- test cast Json to char
select cast(cast('{}' as json) as char(300))

-- test cast Json to nchar
select cast(cast('{}' as json) as nchar(300))

-- test cast Json to varchar
select cast(cast('{}' as json) as varchar(300))

-- test cast Json to nvarchar
select cast(cast('{}' as json) as nvarchar(max))
