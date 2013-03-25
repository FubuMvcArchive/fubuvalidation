COMPILE_TARGET = ENV['config'].nil? ? "Debug" : ENV['config']
CLR_TOOLS_VERSION = "v4.0.30319"

buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]

if( ! buildsupportfiles.any? )
  # no buildsupport, let's go get it for them.
  sh 'git submodule update --init' unless buildsupportfiles.any?
  buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]
end

# nope, we still don't have buildsupport. Something went wrong.
raise "Run `git submodule update --init` to populate your buildsupport folder." unless buildsupportfiles.any?

buildsupportfiles.each { |ext| load ext }

include FileTest
require 'albacore'
load "VERSION.txt"

RESULTS_DIR = "results"
PRODUCT = "FubuValidation"
COPYRIGHT = 'Copyright 2008-2011 Jeremy D. Miller, Josh Arnold, Joshua Flanagan, et al. All rights reserved.';
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';

ARTIFACTS = File.expand_path("artifacts")
@teamcity_build_id = "bt378"
tc_build_number = ENV["BUILD_NUMBER"]
build_revision = tc_build_number || Time.new.strftime('5%H%M')
BUILD_NUMBER = "#{BUILD_VERSION}.#{build_revision}"

props = { :stage => File.expand_path("build"), :artifacts => ARTIFACTS }

desc "**Default**, compiles and runs tests"
task :default => [:compile, :unit_test, :run_jasmine]

desc "Target used for the CI server"
task :ci => [:compile, :unit_test, :run_jasmine_ci, :storyteller_ci, :history, :package]

desc "Target used for the CI on mono"
task :mono_ci => [:compile, :mono_unit_test]

desc "Update the version information for the build"
assemblyinfo :version do |asm|
  asm_version = BUILD_VERSION + ".0"
  
  begin
    commit = `git log -1 --pretty=format:%H`
  rescue
    commit = "git unavailable"
  end
  puts "##teamcity[buildNumber '#{BUILD_NUMBER}']" unless tc_build_number.nil?
  puts "Version: #{BUILD_NUMBER}" if tc_build_number.nil?
  asm.trademark = commit
  asm.product_name = PRODUCT
  asm.description = BUILD_NUMBER
  asm.version = asm_version
  asm.file_version = BUILD_NUMBER
  asm.custom_attributes :AssemblyInformationalVersion => asm_version
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO 
end

desc "Prepares the working directory for a new build"
task :clean => [:update_buildsupport] do

	FileUtils.rm_rf props[:stage]
    # work around nasty latency issue where folder still exists for a short while after it is removed
    waitfor { !exists?(props[:stage]) }
	Dir.mkdir props[:stage]
    
	Dir.mkdir props[:artifacts] unless exists?(props[:artifacts])
end

def waitfor(&block)
  checks = 0
  until block.call || checks >10 
    sleep 0.5
    checks += 1
  end
  raise 'waitfor timeout expired' if checks > 10
end

desc "Sets up the Bottles/Fubu aliases"
task :aliases => [:restore_if_missing] do
	bottles 'alias hellovalidation src/FubuMVC.HelloValidation'
end 

desc "Compiles the app"
task :compile => [:restore_if_missing, :clean, :aliases, :version] do
  bottles("assembly-pak src/FubuMVC.Validation")
  compileSolution COMPILE_TARGET
  copyOutputFiles "src/FubuValidation.StructureMap/bin/#{COMPILE_TARGET}", "Fubu*.{dll,pdb}", props[:stage]  
  copyOutputFiles "src/FubuMVC.Validation/bin", "FubuMVC.Validation.{dll,pdb}", props[:stage]  
end

def compileSolution(target)
  MSBuildRunner.compile :compilemode => target, :solutionfile => 'src/FubuValidation.sln', :clrversion => CLR_TOOLS_VERSION
end

def copyOutputFiles(fromDir, filePattern, outDir)
  Dir.glob(File.join(fromDir, filePattern)){|file| 		
	copy(file, outDir) if File.file?(file)
  } 
end

desc "Runs unit tests"
task :test => [:unit_test]

desc "Run unit tests"
task :unit_test do 
  runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86'
  tests = Array.new
  file = File.new("TESTS.txt", "r")
  assemblies = file.readlines()
  assemblies.each do |a|
	test = a.gsub("\r\n", "").gsub("\n", "")
	tests.push(test)
  end
  file.close
  
  runner.executeTests tests
end

desc "Runs some of the unit tests for Mono"
task :mono_unit_test => :compile do
  runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86'
  runner.executeTests ['FubuValidation.Tests', 'FubuValidation.StructureMap.Tests', 'FubuMVC.Validation.Tests']
end

desc "Runs the StoryTeller UI"
task :run_st => [:restore_if_missing] do
  st = Platform.runtime(Nuget.tool("Storyteller2", "StorytellerUI.exe"))
  sh st 
end

desc "Opens the Serenity Jasmine Runner in interactive mode"
task :open_jasmine do
	serenity "jasmine interactive src/serenity.txt -b Firefox"
end

desc "Runs the Jasmine tests"
task :run_jasmine do
	serenity "jasmine run src/serenity.txt -b Phantom"
end

desc "Runs the Jasmine tests and outputs the results for CI"
task :run_jasmine_ci do
	serenity "jasmine run --verbose --timeout 15 src/serenity.txt -b Phantom"
end

task :storyteller_ci do
	serenity "storyteller src/FubuMVC.Validation.StoryTeller/validation.xml artifacts/Storyteller.html -b Phantom"
end

def self.fubu(args)
  fubu = Platform.runtime(Nuget.tool("FubuMVC.References", "fubu.exe"))
  sh "#{fubu} #{args}" 
end

def self.serenity(args)
  if Platform.is_nix
    puts "Skipping Serentiy. Not currently supported on *nix based systems."
    return
  end
  serenity = Platform.runtime(Nuget.tool("Serenity", "SerenityRunner.exe"))
  sh "#{serenity} #{args}"
end

def self.storyteller(args)
  st = Platform.runtime(Nuget.tool("Storyteller2", "st.exe")) 
  sh "#{st} #{args}"
end

def self.bottles(args)
  bottles = Platform.runtime(Nuget.tool("Bottles", "BottleRunner.exe"))
  sh "#{bottles} #{args}"
end
