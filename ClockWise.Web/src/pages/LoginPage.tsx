import React, { useState } from "react";
import { useParams } from "react-router-dom";
import "./LoginPage.css";


const LoginPage: React.FC = () => {
    const { publicShortName } = useParams(); // Get the company identifier from the URL
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    const handleLogin = async (event: React.FormEvent) => {
        event.preventDefault();
        setError(null);

        try {
            const response = await fetch(`/api/auth/login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email,
                    password,
                    publicShortName, // Include the company's short name in the request
                }),
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || "Failed to log in");
            }

            const data = await response.json();
            console.log("Login successful!", data);
            // Redirect to the dashboard or desired page
        } catch (err: any) {
            setError(err.message);
        }
    };

    return (
        <div style={{ maxWidth: "400px", margin: "0 auto", padding: "20px" }
        }>
            <h1>Login for {publicShortName} </h1>
            < form onSubmit={handleLogin} >
                <div style={{ marginBottom: "15px" }}>
                    <label htmlFor="email" > Email </label>
                    < input
                        type="email"
                        id="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        style={{ width: "100%", padding: "8px" }}
                        required
                    />
                </div>
                < div style={{ marginBottom: "15px" }}>
                    <label htmlFor="password" > Password </label>
                    < input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        style={{ width: "100%", padding: "8px" }}
                        required
                    />
                </div>
                {error && <p style={{ color: "red" }}> {error} </p>}
                <button type="submit" style={{ padding: "10px 20px" }}>
                    Login
                </button>
            </form>
        </div>
    );
};

export default LoginPage;