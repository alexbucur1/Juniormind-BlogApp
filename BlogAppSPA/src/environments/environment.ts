// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  url_api: "http://localhost:5002",
  url_node: "http://localhost:3000",
  url_auth: "https://localhost:5443",
  url_login: 'http://localhost:3001',
  client_id: "Angular.client",
  redirect_uri: "https://localhost:5002/api/Posts",
  response_type: "token id_token",
  scope: "openid user_profile Api.Scope IdentityServerApi",
  state: "123456",
  nonce: "123456",
  response_mode: "form_post",
  auth_secret: 'Angular Secret'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
