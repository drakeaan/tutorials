use azure_core::http::ClientOptions;
use azure_identity::DefaultAzureCredential;
use azure_security_keyvault_secrets::{SecretClient, SecretClientOptions};

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let credential = DefaultAzureCredential::new()?;

    let options = SecretClientOptions {
        api_version: "7.5".to_string(),
        ..Default::default()
    };

    let client = SecretClient::new(
        "https://<your-key-vault-name>.vault.azure.net/",
        credential.clone(),
        Some(options),
    )?;

    Ok(())
}


use azure_core::http::Response;
use azure_identity::DefaultAzureCredential;
use azure_security_keyvault_secrets::{models::Secret, SecretClient};

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    // create a client
    let credential = DefaultAzureCredential::new()?;
    let client = SecretClient::new(
        "https://<your-key-vault-name>.vault.azure.net/",
        credential.clone(),
        None,
    )?;

    // call a service method, which returns Response<T>
    let response = client.get_secret("secret-name", "", None).await?;

    // Response<T> has two main accessors:
    // 1. The `into_body()` function consumes self to deserialize into a model type
    let secret = response.into_body().await?;

    // get response again because it was moved in above statement
    let response: Response<Secret> = client.get_secret("secret-name", "", None).await?;

    // 2. The deconstruct() method for accessing all the details of the HTTP response
    let (status, headers, body) = response.deconstruct();

    // for example, you can access HTTP status
    println!("Status: {}", status);

    // or the headers
    for (header_name, header_value) in headers.iter() {
        println!("{}: {}", header_name.as_str(), header_value.as_str());
    }

    Ok(())
}