write-host "Reverting DB..."
dotnet ef database update 0

write-host "Removing initial migration..."
dotnet ef migrations remove

write-host "Adding initial migration..."
dotnet ef migrations add initial

write-host "Updating DB..."
dotnet ef database update