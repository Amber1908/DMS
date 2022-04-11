const RemoveElements = (array, elements) => {
    let tempAry = array.slice();

    elements.forEach(element => {
        let index = tempAry.indexOf(element);

        if (index > -1) {
            tempAry.splice(index, 1);
        }
    });

    return tempAry;
};

export const fillArray = (value, len) => {
  var arr = [];
  for (var i = 0; i < len; i++) {
    arr.push(value);
  }
  return arr;
}

export { RemoveElements };