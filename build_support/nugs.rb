namespace :nug do
	@nuget = "lib/nuget.exe"
	@dependencies = ["FubuMVC"]
	
	@packname = 'FubuValidation'
	@nugroot = File.expand_path("/nugs")
	
	desc "Build the nuget package"
	task :build do
		sh "#{@nuget} pack packaging/nuget/fubumvc.validation.nuspec -o #{ARTIFACTS} -Version #{BUILD_NUMBER}"
	end

	desc "pulls new NuGet updates from your local machine"
	task :pull, :location do |t, args|
		args.with_defaults(:location => 'local')
		location = args[:location]
		
		@dependencies.each do |fn| 
			sh "#{@nuget} install FubuMVC /Source #{@nugroot} /OutputDirectory lib /ExcludeVersion"
		end
	end
		
	desc "pushes new NuGet udates to your local machine"
	task :push, [:location] => [:build] do |t, args|
		args.with_defaults(:location => 'local')
		location = args[:location]
			
		Dir["#{ARTIFACTS}/*.nupkg"].each do |fn|		
			FileUtils.cp fn, @nugroot
		end
	end
	
end