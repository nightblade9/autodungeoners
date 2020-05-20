// Matches Models.User in C#

export interface DocVersion {
    major: number,
    minor: number,
    revision: number
};

export interface IUser
{
    id: any, // from mongoDB

    version: DocVersion,
    emailAddress: string,
    gold: number
};
