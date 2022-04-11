export const IsNotNullOrEmpty = (string) => {
    return string != null && string !== "";
}

export const IsNullOrEmpty = (string) => {
    return string == null || string === "";
}