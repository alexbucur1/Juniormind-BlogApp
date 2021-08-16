export const environment = {
  production: false,
  url_api: "http://localhost:5002",
  url_node: "http://localhost:3000",
  url_auth: "http://localhost:3001",
  client_id: "Angular.client",
  redirect_uri: "https://localhost:5002/api/Posts",
  response_type: "token id_token",
  scope: "openid user_profile Api.Scope IdentityServerApi",
  state: "123456",
  nonce: "123456",
  response_mode: "form_post",
  auth_secret: 'Angular Secret'
};
