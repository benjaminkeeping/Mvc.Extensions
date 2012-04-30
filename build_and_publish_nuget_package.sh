#!/bin/bash

# project vars
project_name="Mvc.Extensions"
nuget='./tools/nuget.exe'
msbuild='/c/Windows/Microsoft.NET/Framework/v4.0.30319/msbuild.exe'
project_src_dir="./src/${project_name}"
build_mode='Release'
project_output_dir="$project_src_dir/bin/$build_mode"
build_dir="./build"
package_output_dir="./deploy"
package_spec_file="./Package.nuspec"
build_log="$build_dir/build_log.txt"

rm -rf $build_dir
mkdir -p ${build_dir}/lib/net40
rm -rf $package_output_dir
mkdir $package_output_dir

# build project
$msbuild $project_src_dir/${project_name}.csproj /property:Configuration=$build_mode > $build_log

errors=`cat $build_log | grep -i "Build FAILED"`
if [ "" != "${errors}" ]
then
	echo "Build failed"
	exit -1
fi

# increment build version number
currentVersion=`grep version $package_spec_file | cut -d '>' -f2 | cut -d '<' -f1`
echo "Current assembly version $currentVersion"
let patch=`echo $currentVersion | cut -d '.' -f4`+1
suggestedVersion="`echo $currentVersion | cut -d '.' -f1,2,3`.${patch}"
echo "enter new version number eg. $suggestedVersion" [Enter to accept]
read -e newversion
if [ "" == "${newversion}" ]
then
 newversion=$suggestedVersion
fi
echo "Using $newversion"
cat $package_spec_file | sed s/\<version\>[0-9]*.[0-9]*.[0-9]*.[0-9]*\</\<version\>$newversion\</ > tmp.nuspec
mv tmp.nuspec $package_spec_file

cp $project_output_dir/${project_name}.dll $build_dir/lib/net40
cp $package_spec_file $build_dir

$nuget pack ${build_dir}/${package_spec_file} -OutputDirectory $package_output_dir

# push to gallery (requires api key is set on command line)
$nuget push $package_output_dir/${project_name}.$newversion.nupkg