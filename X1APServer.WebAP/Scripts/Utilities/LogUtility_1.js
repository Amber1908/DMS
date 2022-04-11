const Log = (() => {
    const Debug = (msg, traceFlag) => {
        traceFlag = traceFlag == null ? false : traceFlag;

        // console.log(msg);
        // if (traceFlag) console.trace();
    };

    return { Debug }
})();

export default Log;