# [2.1.0](https://github.com/droidsolutions/asp-auth-claim-binder/compare/v2.0.1...v2.1.0) (2024-08-06)


### Features

* add support for Int32 ([54a2466](https://github.com/droidsolutions/asp-auth-claim-binder/commit/54a246600cb601f974ba14cb9059b57af57f72ed))

## [2.0.1](https://github.com/droidsolutions/asp-auth-claim-binder/compare/v2.0.0...v2.0.1) (2024-05-29)


### Bug Fixes

* re-release NuGet because of errors during push ([76eabc9](https://github.com/droidsolutions/asp-auth-claim-binder/commit/76eabc93d687d6d9b14cfa7e00f735c54183d72c))

# [2.0.0](https://github.com/droidsolutions/asp-auth-claim-binder/compare/v1.2.0...v2.0.0) (2024-05-29)


### Features

* add support for .NET 8 ([7858c77](https://github.com/droidsolutions/asp-auth-claim-binder/commit/7858c776daf308138805c0466066994426cf45dc))
* remove obsolete Exception constructor with SerializationInfo from ClaimParsingException ([bfda5ff](https://github.com/droidsolutions/asp-auth-claim-binder/commit/bfda5ff401da6ba56deca9e81657fde456590c62))
* remove obsolete Exception constructor with SerializationInfo from MissingClaimException ([7c98da0](https://github.com/droidsolutions/asp-auth-claim-binder/commit/7c98da0539ebb4252d7b327590104fb50bcfaac2))


### BREAKING CHANGES

* drop support for.NET 7
* removes the constructor overload from ClaimParsingException with SerializationInfo and StreamingContext
See SYSLIB0051
* removes the constructor overload from MissingClaimException with SerializationInfo and StreamingContext
See SYSLIB0051

# [1.2.0](https://github.com/droidsolutions/asp-auth-claim-binder/compare/v1.1.0...v1.2.0) (2023-02-13)


### Features

* update to .NET 7 ([837bf2b](https://github.com/droidsolutions/asp-auth-claim-binder/commit/837bf2ba7aabaaa9f3405930279e4cd1507dd7fb))

# [1.1.0](https://github.com/droidsolutions/asp-auth-claim-binder/compare/v1.0.0...v1.1.0) (2023-01-18)


### Features

* add custom exceptions for model binding errors ([ea8f09b](https://github.com/droidsolutions/asp-auth-claim-binder/commit/ea8f09bafac3e211f7ac7deb997db78715c3c13e))

# 1.0.0 (2022-07-15)


### Features

* initial release ([bdb5342](https://github.com/droidsolutions/asp-auth-claim-binder/commit/bdb5342e4a528d0b79ebbb917fb50e6229c9d351))

# 1.0.0-develop.1 (2022-07-06)


### Features

* initial release ([bdb5342](https://github.com/droidsolutions/asp-auth-claim-binder/commit/bdb5342e4a528d0b79ebbb917fb50e6229c9d351))
