function twoParametersWithSameName(x, x) {
  return x;
}
if(!(twoParametersWithSameName(1, 2) === 2)) {
  $ERROR("#1: twoParametersWithSameName(1, 2) === 2");
}