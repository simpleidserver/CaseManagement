export class CreateHumanTaskInstance {
    humanTaskName: string;
    priority: number;
    activationDeferralTime: Date;
    expirationTime: Date;
    operationParameter: any;
}