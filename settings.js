
// Link to Project Assets
import {config} from './webbuild/Build/buildconfig.js'
import {Unity} from './Unity.js'

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
        {id:'blink', class: brainsatplay.plugins.controls.Event},
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
                    function: 'UpdateAlpha',
                    type: 'number'
                },
                {
                    object: 'GameApplication',
                    function: 'UpdateAlphaBeta',
                    type: 'number'
                },
                {
                    object: 'GameApplication',
                    function: 'UpdateAlphaTheta',
                    type: 'number'
                },
                {
                    object: 'GameApplication',
                    function: 'UpdateCoherence',
                    type: 'number'
                },
                {
                    object: 'GameApplication',
                    function: 'UpdateFocus',
                    type: 'number'
                },
                {
                    object: 'GameApplication',
                    function: 'UpdateThetaBeta',
                    type: 'number'
                },
                {
                    object: 'GameApplication',
                    function: 'UpdateBlink',
                    type: 'boolean'
                },
                {
                    object: 'GameApplication',
                    function: 'UpdateO1',
                    type: 'number'
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
          source: 'eeg:atlas',
          target: 'neurofeedback',
        },
        {
          source: 'neurofeedback',
          target: 'unity:UpdateFocus',
        },

          {
            source: 'blink',
            target: 'unity:UpdateBlink',
          },

        {
          source: 'unity:element',
          target: 'ui:content',
        }
      ]
    },
    connect: true
}