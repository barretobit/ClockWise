import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import LoginPage from "./pages/LoginPage";

function App() {
    return (
        <Router>
            <Routes>
                <Route path=":publicShortName/login" element={<LoginPage />} />
            </Routes>
        </Router>
    );
}

export default App;