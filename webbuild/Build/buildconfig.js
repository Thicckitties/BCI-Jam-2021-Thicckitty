var buildUrl = "./webbuild/Build";
export var config = {
  dataUrl: buildUrl + "/BCI2021.data",
  loaderUrl: buildUrl + "/BCI2021.loader.js",
  frameworkUrl: buildUrl + "/BCI2021.framework.js",
  codeUrl: buildUrl + "/BCI2021.wasm",
//#if MEMORY_FILENAME
 // memoryUrl: buildUrl + "/{{{ MEMORY_FILENAME }}}",
//#endif
//#if SYMBOLS_FILENAME
//  symbolsUrl: buildUrl + "/{{{ SYMBOLS_FILENAME }}}",
//#endif
  streamingAssetsUrl: "StreamingAssets",
  companyName: "{{{ COMPANY_NAME }}}",
  productName: "{{{ PRODUCT_NAME }}}",
  productVersion: "{{{ PRODUCT_VERSION }}}",
};