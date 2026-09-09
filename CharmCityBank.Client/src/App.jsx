import {useState} from 'react'
import './App.css'

function App() {
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [message, setMessage] = useState("")

    async function handleLogin() {
        event.preventDefault();

        const response = await fetch("http://localhost:5169/api/auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                email: email,
                password: password
            })
        });

        if (!response.ok) {
            setMessage("Invalid email or password");
            return;
        }
        const data = await response.json();

        console.log(data)

        localStorage.setItem("token", data.token)
        setMessage("Login succesful");
    }

    return (
        <div>
            <h1>Charm City Bank</h1>

            <form onSubmit={handleLogin}>
                <input
                    type="email"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    placeholder="Email"
                />

                <input
                    type="password"
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    placeholder="Password"
                />

                <button type="submit">
                    Login
                </button>
            </form>

            <p>{message}</p>
        </div>
    )
}

export default App
