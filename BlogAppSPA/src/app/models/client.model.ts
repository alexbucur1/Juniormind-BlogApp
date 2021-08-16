export interface Client {
    token: string,
    userID: string,
    userIsAdmin: boolean,
    fullName: string,
    callBackUrl: string
}