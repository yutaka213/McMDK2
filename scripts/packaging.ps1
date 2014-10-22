
# Create new item
Remove-Item ./Package -Recurse
New-Item ./Package -ItemType directory
$contents = Get-ChildItem *.exe,*.dll -Name
foreach($item in $contents)
{
	Copy-Item $item ./Package
}
Copy-Item ./assets ./Package -Recurse
Remove-Item ./Package/*.vshost.exe

#[System.IO.Compression.Zipfile]::ExtractToDirectory("���k�t�@�C���̐�΃p�X", "�𓀐�̐�΃p�X");