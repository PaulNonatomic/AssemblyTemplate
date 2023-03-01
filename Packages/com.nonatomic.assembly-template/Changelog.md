# Changelog
All notable changes to this package will be documented in this file.
The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/) and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.2.2] - 2023-03-01
- updated the AssemblyTemplateWindow to use revised field names

## [0.2.1] - 2023-02-17
- altered the NewPackageTemplate so the root Assembly Definition files root namespace includes the package name before the directory name.
- added tests to ensure all included templates can be deserialized

## [0.2.0] - 2023-02-16
- extended the package creation window to include all necessary fields for the creation of a new package
- package creation window now creates the package in the packages folder of the project

## [0.1.2] - 2023-02-15
- added support for package

## [0.1.1-preview.1] - 2023-02-11
- split the existing template into two variants: `BasicAssemblyTemplate` and `GameAssemblyTemplate` with the later including some sub folder organisation.

## [0.1.0-preview.1] - 2023-02-08
- initial package setup