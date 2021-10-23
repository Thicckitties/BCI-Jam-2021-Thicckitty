export class Reverse{

    static id = String(Math.floor(Math.random()*1000000))
    
    constructor(label, session) {

        this.label = label
        this.session = session


        this.ports = {
            default: {
                input: {type: undefined},
                output: {type: undefined},
                onUpdate: (user) => {
                    user.data = -user.data
                    return user
                }
            },
        }
    }

    init = () => {

    }

    deinit = () => {

    }
}