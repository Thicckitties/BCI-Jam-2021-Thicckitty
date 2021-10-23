
// Link to Project Assets
import {config} from './webbuild/Build/buildconfig.js'
import {Unity} from './Unity.js'
import {Reverse} from './Reverse.js'

export const settings = {
    name: "Unity Template",
    devices: ["EEG", "HEG"],
    author: "Juris Zebneckis",
    description: "",
    categories: ["WIP"],
    instructions:"",

    // App Logic
    graph:
    {
      nodes: [
        {id:'eeg', class: brainsatplay.plugins.biosignals.EEG},
        {id:'neurofeedback', class: brainsatplay.plugins.algorithms.Neurofeedback, params: {metric: 'Focus'}},
        {id:'kick', class: brainsatplay.plugins.controls.Event, params: {keycode: 'Space'}},
        {id:'up', class: brainsatplay.plugins.controls.Event, params: {keycode: 'w'}},
        {id:'down', class: brainsatplay.plugins.controls.Event, params: {keycode: 's'}},
        {id:'left', class: brainsatplay.plugins.controls.Event, params: {keycode: 'a'}},
        {id:'right', class: brainsatplay.plugins.controls.Event, params: {keycode: 'd'}},
        
        {id:'reverseleft', class: Reverse},
        {id:'reversedown', class: Reverse},

        {id:'debug', class: brainsatplay.plugins.debug.Debug},

        {
          id:'unity', 
          class: Unity, 
          params:{
              config,
              // onUnityEvent: (ev) => { // NOT BEING ASSIGNED

              //   // Parse Messages from Unity
              //   if (typeof ev === 'string'){
              //     console.log('MESSAGE: ' + ev)
              //   }

              //   // Blink the Robot (v0.0.34)
              //   let blink = settings.graph.nodes.find(n => n.id === 'blink');
              //   console.log(blink)
              //   blink.session.graph.runSafe(blink, 'default', {data: true});
              //   blink.session.graph.runSafe(blink, 'default', {data: false});
              // },
              commands: 
              [
                {
                    object: 'GameApplication',
                    function: 'XInput',
                    type: 'boolean'
                },
                {
                    object: 'GameApplication',
                    function: 'YInput',
                    type: 'boolean'
                },
                {
                    object: 'GameApplication',
                    function: 'Kick',
                    type: 'boolean'
                }
            ]
          }
        },
        {
          id:'ui', 
          class: brainsatplay.plugins.interfaces.UI
        }
    ],

      edges: [

        // BRAIN
        {
          source: 'right',
          target: 'unity:XInput',
        },

        {
          source: 'left',
          target: 'reverseleft',
        },

        {
          source: 'reverseleft',
          target: 'unity:XInput',
        },

        {
          source: 'up',
          target: 'unity:YInput',
        },

        {
          source: 'down',
          target: 'reversedown',
        },

        {
          source: 'reversedown',
          target: 'unity:YInput',
        },

        {
          source: 'reversedown',
          target: 'debug',
        },

          {
            source: 'kick',
            target: 'unity:Kick',
          },

        {
          source: 'unity:element',
          target: 'ui:content',
        }
      ]
    },
    connect: true
}