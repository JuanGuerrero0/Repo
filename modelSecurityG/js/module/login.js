document.getElementById("login-form").addEventListener("submit", async function (e) {
    e.preventDefault();

    const username = document.getElementById("login-username").value;
    const password = document.getElementById("login-password").value;

    const loginData = {
        email: username, // Si tu backend usa username, cambia a: username: username
        password: password
    };

    try {
        const response = await fetch("https://localhost:7287/api/Auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(loginData)
        });

        const token = await response.text();

        if (!response.ok) {
            throw new Error("Credenciales incorrectas");
        }

        localStorage.setItem("token", token);
        document.getElementById("login-success").textContent = "¡Inicio de sesión exitoso!";
        document.getElementById("login-error").textContent = "";

        setTimeout(() => {
            window.location.href = "index.html";
        }, 1500);
    } catch (error) {
        document.getElementById("login-error").textContent = error.message;
        document.getElementById("login-success").textContent = "";
    }
});
