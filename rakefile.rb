begin
  require 'bundler/setup'
  require 'fuburake'
rescue LoadError
  puts 'Bundler and all the gems need to be installed prior to running this rake script. Installing...'
  system("gem install bundler --source http://rubygems.org")
  sh 'bundle install'
  system("bundle exec rake", *ARGV)
  exit 0
end

@solution = FubuRake::Solution.new do |sln|
	sln.compile = {
		:solutionfile => 'src/FubuValidation.sln'
	}

	sln.assembly_info = {
		:product_name => "FubuValidaton",
		:copyright => 'Copyright 2008-2013 Jeremy D. Miller, Josh Arnold, et al. All rights reserved.'
	}

	sln.ripple_enabled = true
	sln.fubudocs_enabled = false
    
    sln.assembly_bottle 'FubuMVC.Validation'

	# too many problems right now
    #sln.ci_steps = ['st:run']

	sln.options[:nuget_publish_folder] = 'nupkgs'
	sln.options[:nuget_publish_url] = 'https://www.myget.org/F/fubumvc-edge/'
end

add_dependency 'ripple:publish', 'st:run'

#add_dependency 'ripple:publish', 'run_phantom'

#add_dependency 'ripple:publish', 'st:run'
#add_dependency 'ripple:publish', 'run_phantom'

desc "Target used for the CI on mono"
task :mono_ci => [:compile, :mono_unit_test]

desc "Runs some of the unit tests for Mono"
task :mono_unit_test => :compile do
  runner = NUnitRunner.new :compilemode => @solution.compilemode, :source => 'src', :platform => 'x86'
  runner.executeTests ['FubuValidation.Tests', 'FubuValidation.StructureMap.Tests', 'FubuMVC.Validation.Tests']
end


FubuRake::Storyteller.new({
  :path => 'src/FubuMVC.Validation.StoryTeller',
  :profile => 'Firefox',
  :compilemode => @solution.compilemode
})

FubuRake::Storyteller.new({
  :path => 'src/FubuMVC.Validation.StoryTeller',
  :prefix => 'st:phantom',
  :profile => 'Phantom',
  :compilemode => @solution.compilemode
})

FubuRake::Storyteller.new({
  :path => 'src/FubuMVC.Validation.StoryTeller',
  :prefix => 'st:chrome',
  :profile => 'Chrome',
  :compilemode => @solution.compilemode
})

desc "Smoke test"
task :smoke => [:default] do
    sh "rm -rf src/FubuMVC.Validation.StoryTeller/fubu-content"
    serenity "storyteller src/FubuMVC.Validation.StoryTeller/validation.xml -b Phantom"
end

def self.serenity(args)
  serenity = Platform.runtime(Nuget.tool("Serenity", "SerenityRunner.exe"), "v4.0.30319")
  sh "#{serenity} #{args}"
end
