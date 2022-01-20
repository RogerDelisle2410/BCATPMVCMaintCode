/*
 * Helper to detect borders of a given element
 * @method
 * @memberof Popper.Utils
 * @param {CSSStyleDeclaration} styles
 * Result of `getStyleComputedProperty` on the given element
 * @param {String} axis - `x` or `y`
 * @return {number} borders - The borders size of the given axis
 */

export default function getBordersSize(styles, axis) {
  const sideA = axis === 'x' ? 'Left' : 'Top';
  const sideB = sideA === 'Left' ? 'Right' : 'Bottom';

  return (
<<<<<<< HEAD
    parseFloat(styles[`border${sideA}Width`]) +
    parseFloat(styles[`border${sideB}Width`])
=======
    parseFloat(styles[`border${sideA}Width`], 10) +
    parseFloat(styles[`border${sideB}Width`], 10)
>>>>>>> 11bdd4e... Add project files.
  );
}
