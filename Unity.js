export class Unity{

    static id = String(Math.floor(Math.random()*1000000))
    
    constructor(label, session) {

        this.label = label
        this.session = session

        // Unity instance window callbacks that we call from .jslib in Unity.
        // Modify these signatures to respond to whatever you want to send from Unity.
        window.PassUnityEvent = (param) => {
            this.session.graph.runSafe(this, 'unityEvent', {data: param})
        }

        this.props = {
            instance: null,
            canvas: document.createElement('canvas')
        }

        this.props.canvas.style = `width: 100%; height: 100%;`


        this.ports = {
            element: {
                data: this.props.canvas,
                input: {type: undefined},
                output: {type: Element},
            },
            config: {
                input: {type: null},
                output: {type: null},
            },
            unityEvent: {
                input: {type: null},
                output: {type: 'string'},
                onUpdate: (user) => { 
                    this.ports.onUnityEvent.data(user.data)
                }
            },

            // Declare functions that will be called by Unity instance window callbacks here.
            onUnityEvent: {
                data: (ev) => {
                    console.log("OnUnityEvent with parameters call" + ev)
                },
                input: {type: Function},
                output: {type: 'string'}
            },

            // Declare commands that can be sent to Unity
            commands: {
                data: [],
                input: {type: Array},
                output: {type: null},
            },
        }
    }

    init = () => {

            import(this.ports.config.data?.loaderUrl).then(module => {
                //Add whatever else you need to initialize
                console.log(module)
                module.createUnityInstance(this.props.canvas, this.ports.config.data, (progress) => {}).then((unityInstance) => {
                    this.props.instance = unityInstance;
                }).catch((message) => {alert(message)});
            })


            this.ports.commands.data.forEach(o => {
                this.session.graph.addPort(this, o.function, {
                    input: {type: o.type},
                    output: {type: null},
                    onUpdate: (user) => {
                        // let data = JSON.stringify(user.data)
                        // let data = user.data.toString()
                        let data = user.data
                        if (typeof data === 'boolean') data = (data) ? 1 : 0
                        if (this.props.instance) this.props.instance.SendMessage(o.object, o.function, data);
                    }
                })
            })
    }

    deinit = () => {
       if (this.props.instance.Quit instanceof Function) this.props.instance.Quit()
    }
}