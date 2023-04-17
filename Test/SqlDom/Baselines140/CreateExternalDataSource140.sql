CREATE EXTERNAL DATA SOURCE [My Blob Source]
    WITH (
    TYPE = BLOB_STORAGE,
    LOCATION = 'http://anyrandomlocation'
    );

CREATE EXTERNAL DATA SOURCE [MyBlobSource2]
    WITH (
    TYPE = BLOB_STORAGE,
    LOCATION = 'anything_goes_here',
    CREDENTIAL = [My Credential]
    );

CREATE EXTERNAL DATA SOURCE [My Blob Source 2]
    WITH (
    TYPE = BLOB_STORAGE,
    LOCATION = 'anything_goes_here',
    CREDENTIAL = [My Credential]
    );

CREATE EXTERNAL DATA SOURCE MyBlobSource3
    WITH (
    TYPE = BLOB_STORAGE,
    LOCATION = 'https://a/b/c/d',
    CREDENTIAL = MyCredential
    );

